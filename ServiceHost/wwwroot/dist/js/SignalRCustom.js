﻿
var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

$(document).ready(function () {
    if (Notification.permission !== "granted") {
        Notification.requestPermission();
    }
});

var currentGroupId = 0;
var userId = 0;
var textMessage = "";



connection.on("SetUserId", function (id) {

    userId = id;
})
connection.on("NewGroup", appendGroup);
connection.on("ReciveNotification", reciveNotif)
connection.on("RecieveMessage", reciveMessage)
connection.on("JoinGroup", joined);
connection.on("TypingNotif", isTypingNotif)
connection.on("ChangeStatus", changeStatus)

function changeStatus(userId, status, lastSeenDate) {

    if (status) {
        $(`#statususer-${userId}`).addClass("online");
        $(`#statususer-${userId}`).removeClass("offline");

        $(`#statususerinchat-${userId}`).addClass("online");
        $(`#statususerinchat-${userId}`).removeClass("offline");
        $(`#lastseendate-${userId}`).text("Active Now");


    } else {
        $(`#statususer-${userId}`).addClass("offline");
        $(`#statususer-${userId}`).removeClass("online");

        $(`#statususerinchat-${userId}`).addClass("offline");
        $(`#statususerinchat-${userId}`).removeClass("online");
        $(`#lastseendate-${userId}`).text(lastSeenDate);
    }
}
function isTypingNotif(user, isTypingMood, isGroupPrivate) {

    if ($(`#typing${user.id}`).length) {


        if (!isTypingMood) {
            $(`#typing${user.id}`).remove();

        }

    }
    else {

        if (isGroupPrivate) {
            $("#chatbody").append(`<div id="typing${user.id}" class="message">
												<img class="avatar-md" src="/UploderFiles/${user.picture}" data-toggle="tooltip" data-placement="top" title="" >
												<div class="text-main">
													<div class="text-group">
														<div class="text typing">
															<div class="wave">
																<span class="dot"></span>
																<span class="dot"></span>
																<span class="dot"></span>
															</div>
														</div>
													</div>
												</div>
											</div>`)
        }

    }



};
function reciveMessage(result) {

    $(`#typing${result.userId}`).remove();

    if (userId == result.userId) {

        if (result.fileAttach != "No File") {
            $("#chatbody").append(`<div class="message me">
                                <div class="text-main">
                                    <div class="text-group me">
                                        <div class="text me">
                                             <div class="attachment">
                                             <a href="/UploderFiles/${result.fileAttach}" download> 
                                             <button class="btn attach"><i class="material-icons md-18">insert_drive_file</i></button>
                                             </a>
                                  
                               <div class="file">
                               <h5><a href="/UploderFiles/${result.fileAttach}" download>Show File</a></h5>
                                 </div>
                                  </div>

                                        </div>
                                    </div>
                                    <span>${result.creationDate}</span>
                                </div>
                            </div>`)
        } else {
            $("#chatbody").append(`<div class="message me">
                                <div class="text-main">
                                    <div class="text-group me">
                                        <div class="text me">
                                            <p>${result.body}</p>
                                        </div>
                                    </div>
                                    <span>${result.creationDate}</span>
                                </div>
                            </div>`)

        }


    }
    else {
        if (result.fileAttach != "No File") {
            $("#chatbody").append(
                `<div class="message">
                                <img class="avatar-md" src="/UploderFiles/${result.avatarSender}" data-toggle="tooltip" data-placement="top" >
                                <div class="text-main">
                                    <div class="text-group">
                                    <h6 style="color:#99377b;">${result.userNameSender}</h6>
                                        <div class="text">
                                     <div class="attachment">
                                    <a href="/UploderFiles/${result.fileAttach}" download> 
                                             <button class="btn attach"><i class="material-icons md-18">insert_drive_file</i></button>
                                             </a>
                               <div class="file">
                               <h5><a href="/UploderFiles/${result.fileAttach}" download>Show File</a></h5>
                                 </div>
                                  </div>
                                        </div>
                                    </div>
                                    <span>${result.creationDate}</span>
                                </div>
                            </div>`)
        } else {
            $("#chatbody").append(
                `<div class="message">
                                <img class="avatar-md" src="/UploderFiles/${result.avatarSender}" data-toggle="tooltip" data-placement="top" >
                                <div class="text-main">
                                    <div class="text-group">
                                    <h6 style="color:#99377b;">${result.userNameSender}</h6>
                                        <div class="text">
                                            <p>${result.body}</p>
                                        </div>
                                    </div>
                                    <span>${result.creationDate}</span>
                                </div>
                            </div>`)
        }

    }


}
function reciveNotif(result) {
    if (Notification.permission === "granted") {
        if (currentGroupId !== result.groupId) {
            var notification = new Notification(result.groupName,
                {
                    result: result.body
                });

        }
    }
}
function joined(group, chats) {

    $("#startchat").css("display", "none");
    $("#chat").css("display", "block");



    if (group.reciverId > 0) {
        if (group.ownerId == userId) {
            $("#imagegroup").attr("src", `/UploderFiles/${group.reciverPicture}`)
            $("#chatname").html(group.reciverUserName);

            $(`#lastseendate-${group.reciverId}`).remove();

            var status = "";
            if (group.reciverIsOnline) {



                status = "online";
                $("#chatinfo").append(`<span id="lastseendate-${group.reciverId}">Active Now</span>`);
            } else {
                status = "offline";
                $("#chatinfo").append(`<span id="lastseendate-${group.reciverId}">${group.reciverLastSeenDate}</span>`);

            }
            $("#checkstatususer").append(`
                <div  class="status">
                    <i id="statususerinchat-${group.reciverId}" class="material-icons ${status}">fiber_manual_record</i>
               </div>
            `)
        } else {
            $("#imagegroup").attr("src", `/UploderFiles/${group.ownerPicture}`)
            $("#chatname").html(group.ownerUserName);
            $(`#lastseendate-${group.ownerId}`).remove();


            var status = "";
            if (group.ownerIsOnline) {
                status = "online";
                $("#chatinfo").append(`<span id="lastseendate-${group.ownerId}">Active Now</span>`);

            } else {
                status = "offline";
                $("#chatinfo").append(`<span id="lastseendate-${group.ownerId}">${group.ownerLastSeenDate}</span>`);

            }
            $("#checkstatususer").append(`

                <div  class="status">
                    <i id="statususerinchat-${group.ownerId}" class="material-icons ${status}">fiber_manual_record</i>
               </div>
               
            `)
        }

    } else {

        $("#checkstatususer").html('');
        $("#chatinfo").html('');

        $("#imagegroup").attr("src", `/UploderFiles/${group.picture}`)
        $("#chatname").html(group.groupTitle)
    }

    currentGroupId = group.id;

    $("#chatbody").html('');
    for (var i in chats) {

        if (chats[i].userId == userId) {

            if (chats[i].fileAttach != "No File") {
                $("#chatbody").append(`<div class="message me">
                                <div class="text-main">
                                    <div class="text-group me">
                                        <div class="text me">
                                             <div class="attachment">
                                             <a href="/UploderFiles/${chats[i].fileAttach}" download> 
                                             <button class="btn attach"><i class="material-icons md-18">insert_drive_file</i></button>
                                             </a>
                                  
                               <div class="file">
                               <h5><a href="/UploderFiles/${chats[i].fileAttach}" download>Show File</a></h5>
                                 </div>
                                  </div>
                                        </div>
                                    </div>
                                    <span>${chats[i].creationDate}</span>
                                </div>
                                </div>`)
            } else {
                $("#chatbody").append(`<div class="message me">
                                <div class="text-main">
                                    <div class="text-group me">
                                        <div class="text me">
                                            <p>${chats[i].body}</p>
                                        </div>
                                    </div>
                                    <span>${chats[i].creationDate}</span>
                                </div>
                                </div>`)
            }



        }
        else {

            if (chats[i].fileAttach != "No File") {
                $("#chatbody").append(
                    `<div class="message">
                                <img class="avatar-md" src="/UploderFiles/${chats[i].avatarSender}" data-toggle="tooltip" data-placement="top" >
                                <div class="text-main">
                                    <div class="text-group">
                                    <h6 style="color:#99377b;">${chats[i].userNameSender}</h6>
                                        <div class="text">
                                           <div class="attachment">
                                    <a href="/UploderFiles/${chats[i].fileAttach}" download> 
                                             <button class="btn attach"><i class="material-icons md-18">insert_drive_file</i></button>
                                             </a>
                               <div class="file">
                               <h5><a href="/UploderFiles/${chats[i].fileAttach}" download>Show File</a></h5>
                                 </div>
                                  </div>
                                        </div>
                                    </div>
                                    <span>${chats[i].creationDate}</span>
                                </div>
                            </div>`)
            } else {
                $("#chatbody").append(
                    `<div class="message">
                                <img class="avatar-md" src="/UploderFiles/${chats[i].avatarSender}" data-toggle="tooltip" data-placement="top" >
                                <div class="text-main">
                                    <div class="text-group">
                                    <h6 style="color:#99377b;">${chats[i].userNameSender}</h6>
                                        <div class="text">
                                            <p>${chats[i].body}</p>
                                        </div>
                                    </div>
                                    <span>${chats[i].creationDate}</span>
                                </div>
                            </div>`)
            }

        }
    }


}
function appendGroup(groupTitle, picture, id, isPrivate) {



    if (groupTitle == null) {
        alert("We could not add this group")
    }
    else {
        $("#chats").show();
        $("#searchbar").hide();
        $("#searchinput").val('');

        if (isPrivate) {

            $("#chats").append(` <a  class="filterDiscussions all unread single " id="list-chat-list" data-toggle="list"  onclick="JoinInPrivateGroup('${id}')" role="tab">
                                <img class="avatar-md" src="/UploderFiles/${picture}" data-toggle="tooltip" data-placement="top" title="Janette" alt="avatar">

                                <div class="data">
                                    <h5>${groupTitle}</h5>
                                    <span>user</span>
                                </div>
                            </a>`)



        } else {
            $("#chats").append(` <a  class="filterDiscussions all unread single " id="list-chat-list" data-toggle="list"  onclick="JoinInGroup('${id}')" role="tab">
                                <img class="avatar-md" src="/UploderFiles/${picture}" data-toggle="tooltip" data-placement="top" title="Janette" alt="avatar">

                                <div class="data">
                                    <h5>${groupTitle}</h5>
                                    <span>group</span>
                                </div>
                            </a>`)
        }
        $("#close").click();
    }


}
function JoinInGroup(groupId) {


    connection.invoke("JoinPublicGroup", groupId, currentGroupId);
}
function JoinInPrivateGroup(reciverId) {

    connection.invoke("JoinPrivateGroup", reciverId, currentGroupId);

}
function istyping(event) {

    var isTypingMood = false;
    var value = $("#textmessage").val();
    if (value != "") {
        isTypingMood = true;
    }

    connection.invoke("IsTyping", currentGroupId, isTypingMood);
}

connection.start();


function sendmessage(event) {
    event.preventDefault();

    var textmessage = event.target[0].value;
    var fileattach = event.target[3].files[0];

    if (textmessage == "" && fileattach != null) {
        textmessage = "send File";
    }


    var data = new FormData();
    data.append("body", textmessage);
    data.append("currentGroupId", currentGroupId);
    data.append("file", fileattach);



    $.ajax({
        url: "/api/Group/SendMessage",
        type: "POST",
        data: data,
        encytype: "multipart/form-data",
        processData: false,
        contentType: false
    }).done(function () {

        $("#fileattach").val('');
        $("#textmessage").val('');
    });








}
function insertGroup(event) {
    event.preventDefault();

    var groupName = event.target[0].value;
    var imageFile = event.target[1].files[0];

    var formData = new FormData();
    formData.append("GroupName", groupName);
    formData.append("ImageFile", imageFile);

    $.ajax({
        url: "/api/Group/CreateGroup",
        type: "POST",
        data: formData,
        encytype: "multipart/form-data",
        processData: false,
        contentType: false
    })


}
function search() {

    var value = $("#searchinput").val();

    if (value) {
        $("#chats").hide();
        $("#searchbar").show();

        $.ajax({
            url: "/api/Group/Search?title=" + value,
            type: "GET",

        }).done(function (data) {



            $("#searchbar").html("");

            for (var i in data) {



                if (data[i].isUser) {
                    $("#searchbar").append(` <a  class="filterDiscussions all unread single"  onclick="JoinInPrivateGroup('${data[i].id}')" id="list-chat-list" data-toggle="list" role="tab" >
                                <img class="avatar-md" src="/UploderFiles/${data[i].picture}" data-toggle="tooltip" data-placement="top">
                                <div class="data">
                                    <h5>${data[i].title}</h5>
                                 
                                </div>
                            </a>`);
                } else {
                    $("#searchbar").append(` <a  class="filterDiscussions all unread single " onclick="JoinInGroup('${data[i].id}')" id="list-chat-list" data-toggle="list" role="tab" >
                                <img class="avatar-md" src="/UploderFiles/${data[i].picture}" data-toggle="tooltip" data-placement="top" >
                                <div class="data">
                                    <h5>${data[i].title}</h5>
                                 
                                </div>
                            </a>`);
                }

            }


        });
    } else {
        $("#chats").show();
        $("#searchbar").hide();
    }
}


