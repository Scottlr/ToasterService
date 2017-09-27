# Clustered ToasterService
Ammended base of the ToasterService to utilise ZooKeeper for clustered deployment in a swarm cluster

## Guide To Deploy Via Docker Swarm
### Create Registry In Swarm
This is so all nodes in the swarm can pull the image from somewhere.

unless your shell is pointing to a manager node in your cluster, run 
`docker-machine ssh {managerNode} "docker service create --name registry --publish 5000:5000 registry"`

We can now continue to building, tagging and pushing to this repository.

### Push Image To Registry
In \ToasterService, run `docker-compose build` to build the image, then `docker tag toasterservice localhost:5000/toasterservice` to tag that image. Follow that up by pushing it to our registry we created `docker push localhost:5000/toasterservice`.
