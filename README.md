# Divine-Monad

## Installing
Before running application in Visual Studio, do the following steps:

1. Create New SQL Server Database Project
2. Open View: "Sql Server Object Explorer"
3. Click right on new created DataBase (on localdb)
4. Select "Publish Data-tier Application.."
5. In "File on disk" choose file from this project "DivineMonadDataBase.dacpac and click "Publish"
6. Wait several minutes for process to end
7. Copy Connection String of created DataBase
8. Go to file appsettings.json and change "LocalConnection" string
9. Build and run application!
