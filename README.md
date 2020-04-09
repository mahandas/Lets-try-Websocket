# What are WebSockets?
  
The WebSocket Protocol is a widely supported open standard for developing real-time applications. Previous methods for simulating full-duplex connections were based on polling, a synchronous method wherein the client makes a request to the server to see if there is any information available. The client receives a response from the server even if there is no information available.
  
Polling works well for cases where the exact interval of message availability is known. However, in most real-time applications, message frequency is often unpredictable. In addition, polling requires the client to open and close many unnecessary connections.
  
Long polling (also known as Comet) is another popular communication method in which the client opens a connection with the server for a set duration. If the server does not have any information, it holds the request open until it has any information for the client, or until it reaches the end of a designated time limit (timeout). Essentially, Comet delays the completion of the HTTP response until the server has something to send to the client, a technique often called a hanging-GET or pending-POST.
  
The fact that the client has to constantly reconnect to the server for new information makes long polling a bad choice for many real-time applications.
  
The WebSocket Protocol is included in the Connectivity section of the HTML5 specification. It allows the creation of full-duplex, bidirectional connections between a client and a server over the web. It provides a way to create persistent, low latency connections that support transactions handled by either the client or the server. Using WebSockets, one can create truly real-time applications such as chat, collaborative document editing, stock trading applications, and multiplayer online games.
  
image.png
  
  
# Working of Websockets
  
The client establishes a WebSocket connection through a process known as the WebSocket handshake. This process starts with the client sending a regular HTTP request to the server. An Upgrade header is included in this request that informs the server that the client wishes to establish a WebSocket connection.
  
WebSocket URLs use the ws scheme. There is also wss for secure WebSocket connections which is the equivalent of HTTPS.
  
image.png
  
Here is a simplified example of the initial request headers.
  
  
GET ws://websocket.example.com/ HTTP/1.1  
Origin: http://example.com  
Connection: Upgrade  
Host: websocket.example.com  
Upgrade: websocket  
  
If the server supports the WebSocket protocol, it agrees to the upgrade and communicates this through an Upgrade header in the response.
  
 
HTTP/1.1 101 WebSocket Protocol Handshake  
Date: Wed, 16 Oct 2013 10:07:34 GMT  
Connection: Upgrade  
Upgrade: WebSocket  
  
Now that the handshake is complete the initial HTTP connection is replaced by a WebSocket connection that uses the same underlying TCP/IP connection.  
  
At this point either party can start sending data.
  
# WebSocket Server
  
Often, a reverse proxy such as an HTTP server is used to detect WebSocket handshakes, process them, and send those clients to a real WebSocket server. The upside of this is that you can excuse your server from the clutter of handling cookies and authentication handlers. A WebSocket server can be built using many libraries across programming languages:

Javascript:  
  
 Ø  Socket.io  

 Ø  ws  

 Ø  WebSocket-Node  
  
Ruby:  
  
 Ø  EventMachine  

 Ø  Faye  
 
Python:  
  
 Ø  pyWebSocket  

 Ø  tornado  
  
C++:  
  
 Ø  uWebSockets  
  
C#:  
  
 Ø  Fleck  
  
Java:  
  
 Ø  Jetty  
  
.NET:  
  
 Ø  SuperWebSocket  
  
GoLang:  
  
 Ø  Gorilla  
