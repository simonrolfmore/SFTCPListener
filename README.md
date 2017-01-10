# SFTCPListener
A very basic example of a Service Fabric TCP Listener.

This is intended to give a quick start for anyone looking to work with [Service Fabric](https://docs.microsoft.com/en-us/azure/service-fabric/) when using a TCP listener, which isn't currently well documented. 

It is expected that anyone using this knows what they want to do with their listener, and has experience of working with TCP in their own applicatons: all this does is set up the listener infrastructure. 

You can telnet to the port to prove the listener is open, but as there's no code to do anything with the socket, data doesn't actually go anywhere.
