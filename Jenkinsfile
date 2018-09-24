node {
   stage('clone repo') { 
      sh 'rm -r ApiGateway'
      sh 'git clone git@github.com:shahedk/ApiGateway'
      sh 'ls ApiGateway'
    }
    stage('build docker image') { 
      sh "docker build -t shahedk/apigateway:latest ./ApiGateway/"
    }
    stage('push to docker') { 
      sh "docker push shahedk/apigateway:latest"
    }
}
