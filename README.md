# autocomplete
auto complete with client/server/db and wrapped with docker, based on trie struct

In order to run the project, follow those steps:
- verify you have docker installed
- navigate to client folder
- run build-frontend.bat, it will build the frontend app image
- navigate to the server/Server folder 
- run the build-backend.bat, it will build the backend app image
- navigate back to the root folder of the repo
- run via cmd: docker-compose up
- open your browser at http://localhost:8888/autocomplete
- type name of cities and see how the autocomplete is working !
