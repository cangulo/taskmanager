# Execute newman locally
newman run ./Testing/Postman/TaskManager-Process-Tests.postman_collection.json -e ./Testing/Postman/TaskManager.postman_environment.json --insecure

# Execute newman and generate reports locally
newman run ./Testing/Postman/TaskManager-Process-Tests.postman_collection.json -e ./Testing/Postman/TaskManager.postman_environment.json --insecure -r htmlextra --reporter-htmlextra-export ./Testing/PostmanTestResults.html

# Execute The project locally
dotnet run --project ./TaskManagerAPI/TaskManagerAPI.csproj -c Debug --launch-profile TaskManagerAPI

# Execute The .NET Core Project
dotnet run --project ./TaskManagerAPI/TaskManagerAPI.csproj -c Debug --launch-profile TaskManagerAPI


newman run ./Testing/Postman/TaskManager-Process-Tests.postman_collection.json -e "./Testing/Postman/TaskManager Task Example 1.postman_environment.json" --env-var AuthorId=1 --insecure
newman run ./Testing/Postman/TaskManager-Process-Tests.postman_collection.json -e "./Testing/Postman/TaskManager Task Example 2.postman_environment.json" --env-var AuthorId=2 --insecure
newman run ./Testing/Postman/TaskManager-Process-Tests.postman_collection.json -e "./Testing/Postman/TaskManager Task Example 3.postman_environment.json" --env-var AuthorId=3 --insecure

