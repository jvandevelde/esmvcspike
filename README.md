# Elasticsearch MVC Spike

## Setup
1. Install Docker for windows if you don't have access to Elasticsearch server
  - Getting started with Docker for Windows - https://docs.docker.com/engine/installation/windows/
  - Download - https://www.docker.com/products/docker-toolbox
2. Run the following command to spin up a Elasticsearch container of the ```million12/elasticsearch``` image from https://hub.docker.com/r/million12/elasticsearch/
  
  ```
  docker run \
    -d \ 
    --name elasticsearch \
    -p 9200:9200 \
    million12/elasticsearch
  ```
  
3. Run ```docker ps``` to verify that your container is running
4. If it is, you should be able to browse to the Elasticsearch REST API at http://<container ip>:9200
  
