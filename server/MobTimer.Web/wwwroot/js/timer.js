"use strict";

var connection = new signalR.HubConnectionBuilder()
    .withUrl("/timer").build();

var memberChangedHandlers = [];

connection.on("ReceiveMessage", function (user, message) {
   var msg = message.replace(/&/g, "&amp;")
       .replace(/</g, "&lt;").replace(/>/g, "&gt;");
   var encodedMessage = user + " says " + msg;
   var li = document.createElement("li");
   li.textContent = encodedMessage;
   document.getElementById("messagesList").appendChild(li);
});

connection.start().then(function() {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event){
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message).catch(function (err){
        return console.error(err.toString());
    });
    event.preventDefault();
});

connection.on("MembersUpdated", function(members){
   memberChangedHandlers.forEach(function(handler) {
       handler(members);
   })
});

function registerMemberChangedHandler(handler) {
    memberChangedHandlers.push(handler);
}

function registerUser(userName) {
    return connection.invoke("JoinMob", userName)
}
