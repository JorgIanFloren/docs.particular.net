startcode PostgreSql_SagaGetBySagaIdSql

select
    "Id",
    "SagaTypeVersion",
    "Concurrency",
    "Metadata",
    "Data"
from public."EndpointName_SagaName"
where "Id" = @Id

endcode
