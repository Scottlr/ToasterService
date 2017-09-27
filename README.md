# Clustered ToasterService
Ammended base of the ToasterService to utilise ZooKeeper for clustered deployment in a swarm cluster.

## Guide To Deploy Via Docker Swarm

I will be under the assumption that you're pointing your shell to your manager node. This can be done by running
`docker-machine env {managerNodeName}` 
then copy, paste and run the last command in the output of that command.

### Create Registry In Swarm

We need to deploy a registry so all nodes in the swarm can pull the image from somewhere. This registry is local to the swarm, so all nodes can access it.
run `docker service create --name registry --publish 5000:5000 registry"`
We can now continue to building, tagging and pushing to this repository.

### Push Image To Registry

In \ToasterService, run `docker-compose build` to build the image, then `docker tag toasterservice localhost:5000/toasterservice` to tag that image. Follow that up by pushing it to our registry we created `docker push localhost:5000/toasterservice`.

### Deploy To Swarm

Now our registry has our image, we can start deploying ToasterService to all the nodes.
You can deploy it to your swarm by running `docker stack deploy -c docker-stack.yml toasterservice`.

### Verify It's Running

By running `docker stack ps -f "desired-state=running" toasterservice` you can check the running nodes, don't worry if you see errored tasks when running it without the filter, this will mainly be because ZooKeeper hasn't started yet, so it keeps attempting to deploy the container until it connects.


