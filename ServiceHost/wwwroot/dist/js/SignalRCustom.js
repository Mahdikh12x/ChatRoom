


var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
var currentGroupId = 0;
var userId = 0;

connection.on("SendClientMessage", function () {
    console.log("succses");
})


connection.on("SetUserId", function (id) {

    userId = id;
    console.log(userId);
})
connection.on("NewGroup", appendGroup);
connection.on("RecieveMessage", reciveMessage)
connection.on("JoinGroup", joined);


function reciveMessage(result) {


    if (userId == result.userId) {

        
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
    else {

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

function joined(group, chats) {

    $("#startchat").css("display", "none");
    $("#chat").css("display", "block");
    $(".inside img").attr("src", `/UploderFiles/${group.picture}`)
    $("#chatname").html(group.groupTitle)
    currentGroupId = group.id;

    $("#chatbody").html('');
    for (var i in chats) {

        if (chats[i].userId == userId) {

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
        else {

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
function appendGroup(groupTitle, picture, id) {


    if (groupTitle == null) {
        alert("We could not add this group")
    }
    else {
        $("#chats").show();
        $("#searchbar").hide();
        $("#chats").append(` <a  class="filterDiscussions all unread single " id="list-chat-list" data-toggle="list"  onclick="JoinInGroup('${id}')" role="tab">
                                <img class="avatar-md" src="/UploderFiles/${picture}" data-toggle="tooltip" data-placement="top" title="Janette" alt="avatar">
                                <div class="status">
                                    <i class="material-icons online">fiber_manual_record</i>
                                </div>
                                <div class="new bg-yellow">
                                    <span>+7</span>
                                </div>
                                <div class="data">
                                    <h5>${groupTitle}</h5>
                                    <span>Mon</span>
                                    <p>A new feature has been updated to your account. Check it out...</p>
                                </div>
                            </a>`)



        $("#close").click();
    }


}
function JoinInGroup(groupId) {


    connection.invoke("JoinPublicGroup", groupId, currentGroupId);
}

connection.start();


function sendmessage(event) {
    event.preventDefault();

    var textmessage = $("#textmessage").val();




    var data = new FormData();
    data.append("body", textmessage);
    data.append("currentGroupId", currentGroupId);



    $.ajax({
        url: "/api/Group/SendMessage",
        type: "POST",
        data: data,
        encytype: "multipart/form-data",
        processData: false,
        contentType: false
    });

    var textmessage = $("#textmessage").val('');


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
                    $("#searchbar").append(` <a  class="filterDiscussions all unread single  onclick="JoinInGroup('${data[i].id}')" id="list-chat-list" data-toggle="list" role="tab" >
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



