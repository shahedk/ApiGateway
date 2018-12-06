node {
   def mvnHome
   stage('clone repo') { 
      sh 'rm -r ApiGateway'
      sh 'git clone git@github.com:shahedk/ApiGateway'
      sh 'ls ApiGateway'
    }
    stage('build docker image') {
      sh "docker pull microsoft/dotnet:sdk"
      sh "docker pull microsoft/dotnet:aspnetcore-runtime"
      sh "docker build -t shahedk/apigateway:latest ./ApiGateway/"
    }
    stage('push to docker') { 
      sh "docker push shahedk/apigateway:latest"
    }
    stage('deploy') { 
      sh "cd ./ApiGateway/ && docker-compose down"
      sh "cd ./ApiGateway/ && docker-compose pull"
      sh "cd ./ApiGateway/ && docker-compose up -d"
    }
}
