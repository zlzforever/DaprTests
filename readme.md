###

dapr run --dapr-http-port 3500 --dapr-grpc-port 50001 --app-port 5114 --app-id api1 --log-level debug --placement-host-address 192.168.31.78  -- dotnet run