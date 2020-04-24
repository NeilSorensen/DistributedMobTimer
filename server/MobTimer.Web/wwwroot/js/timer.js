"use strict";

var connection = new signalR.HubConnectionBuilder()
    .withUrl("/timer").build();

var memberChangedHandlers = [];
var timerTockedHandlers = [];

connection.start().then(function() {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

connection.on("MembersUpdated", function(members){
   memberChangedHandlers.forEach(function(handler) {
       handler(members);
   });
});

connection.on("Tock", function(timerState) {
    console.log('Timer tock recieved', timerState)
    timerTockedHandlers.forEach(function(handler) {
        handler(timerState);
    });
});

function registerMemberChangedHandler(handler) {
    memberChangedHandlers.push(handler);
}

function registerTimerTocked(handler) {
    timerTockedHandlers.push(handler);
}

function registerUser(userName) {
    return connection.invoke("JoinMob", userName);
}

function startTimer() {
    return connection.invoke('StartDriving');
}

connection.onClose(error => {
    console.assert(connection.state === signalR.HubConnectionState.Disconnected);
})
