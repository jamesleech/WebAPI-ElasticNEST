using System;
using CountDownAPI.Services;
using Nest;

namespace CountDownAPI.Elastic
{
    public abstract class ElasticRepository <T> where T : ElasticBase
    {
        protected ILog _logger { get; set; }
        private string _ESAddress;
        private string _ESIndex;
        private Lazy<ElasticClient> _elasticClient;

        protected ElasticClient elastic {
            get 
            {
                return _elasticClient.Value;
            }
        }

        static ElasticClient InitElasticClient(string address, string index) 
        {
            var node = new Uri(address);
            var settings = new ConnectionSettings(node);
            settings.DefaultIndex(index);
            settings.DisableDirectStreaming(); //DEBUGGING
            return new ElasticClient(settings);
        }
        
        public ElasticRepository(string ESAddress, string ESIndex) 
        {
            _ESAddress = ESAddress;
            _ESIndex = ESIndex.ToLower(); //indices must be lower case
            _elasticClient = new Lazy<ElasticClient>(() => {
                return InitElasticClient(_ESAddress, _ESIndex);
            });
        }


        protected ISearchResponse<T> search(int start = 0, int total = 10) {
            //gets all from this index and type
            return elastic.Search<T>(s=>s
                .From(start).Size(total)
                ); //from and size can be used for paging.
        }
        
        protected bool Exists(string key)
        {
            var item = getByKey(key);
            if(item.IsValid && item.Source.Key == key) {
                return true;
            } else {
                return false;
            }
        }
        
        protected IGetResponse<T> getByKey(string key)
        {
            _logger.Log($"searchByKey: {key}");
            
            if(string.IsNullOrWhiteSpace(key)){
                return null;
            }
            
            var result = elastic.Get<T>(key);
                        
            if(!result.IsValid)
            {
                _logger.Log($"Find invalid search: {key}");
                _logger.Log($"{result.DebugInformation.ToString()}");
            } 

            return result;
        }
        

        protected bool add(T newItem)
        {
            newItem.LastUpdate = DateTime.UtcNow;
            newItem.LastUpdatedBy = "TODO";
            
            if(string.IsNullOrWhiteSpace(newItem.Key))
            {
                return false; //todo, better return values.
            }
            
            //TODO: test if it already exists, add with versioning
            
            var index = elastic.Index(newItem, i=>i
                .Id(newItem.Key) //use the key as the es id
                .Refresh()
            ); 
            
            return index.IsValid;
        }

        protected bool update(T updateItem)
        {
            //TODO: adding again overwrites, should do better, versioning/audit trail etc.
            return this.add(updateItem);
        }
    }
}