
var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

$(document).ready(function () {
    if (Notification.permission !== "granted") {
        Notification.requestPermission();
    }
});

var currentGroupId = 0;
var userId = 0;



connection.on("SetUserId", function (id) {

    userId = id;
})
connection.on("NewGroup", appendGroup);
connection.on("ReciveNotification", reciveNotif)
connection.on("RecieveMessage", reciveMessage)
connection.on("JoinGroup", joined);


function reciveMessage(result) {

    debugger;
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
            $(".inside img").attr("src", `/UploderFiles/${group.reciverPicture}`)
            $("#chatname").html(group.reciverUserName)
        } else {
            $(".inside img").attr("src", `/UploderFiles/${group.ownerPicture}`)
            $("#chatname").html(group.ownerUserName)
        }

    } else {
        $(".inside img").attr("src", `/UploderFiles/${group.picture}`)
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
                                    <span>Mon</span>
                                    <p>A new feature has been updated to your account. Check it out...</p>
                                </div>
                            </a>`)



        } else {
            $("#chats").append(` <a  class="filterDiscussions all unread single " id="list-chat-list" data-toggle="list"  onclick="JoinInGroup('${id}')" role="tab">
                                <img class="avatar-md" src="/UploderFiles/${picture}" data-toggle="tooltip" data-placement="top" title="Janette" alt="avatar">

                                <div class="data">
                                    <h5>${groupTitle}</h5>
                                    <span>Mon</span>
                                    <p>A new feature has been updated to your account. Check it out...</p>
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

connection.start();


function sendmessage(event) {
    event.preventDefault();

    var textmessage = event.target[0].value;
    var fileattach = event.target[3].files[0]
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
    });

    $("#fileattach").val('');
    $("#textmessage").val('');


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



