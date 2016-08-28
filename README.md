#NOTE - EXPERIMENTAL BRANCH - See Dev for reliable branch
The code in this branch is me playing around with .NET Core Beta as well as Elasticsearch NEST 2.x

# Elasticsearch MVC Spike

This is a simple repository showing how Elasticsearch can be used to implement search in a ASP.NET MVC application.

## Setup
To run the web application, you will need to have access to an Elasticsearch server. The simplest way to set one up is to use the official Docker container on your favorite OS. To get Elasticsearch running in Docker:
1. Install the Docker Toolbox for Windows (or your own OS) 
  - Getting started with Docker for Windows - https://docs.docker.com/engine/installation/windows/
  - Download - https://www.docker.com/products/docker-toolbox
2. Once you have Docker installed, run the following command to start a basic, single node Elasticsearch container of the ```million12/elasticsearch``` image from https://hub.docker.com/r/million12/elasticsearch/
  
  ```
  docker run \
    -d \ 
    --name elasticsearch \
    -p 9200:9200 \
    million12/elasticsearch
  ```
  
3. Run ```docker ps``` to verify that your container is running
4. If it is, you should be able to browse to the Elasticsearch REST API at http://\<DOCKER MACHINE IP\>:9200
  
