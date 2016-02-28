# WebAPI-ElasticNEST
Web API with Elasticsearch as store

a simple CRUD web api using elasticsearch as data store.

Uses elasticsearch.net Nest nuget package for abstraction. the plumbing of this is then abstracted into the generic abstract ElasticRepository<T>. 

See both TeamRepository and ReleaseRepositoy for example usage.
