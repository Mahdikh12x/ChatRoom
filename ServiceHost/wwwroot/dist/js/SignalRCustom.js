

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
var currentGroupId = 0;


connection.on("SendClientMessage", function () {
    console.log("succses");
})


connection.on("NewGroup", appendGroup);
connection.on("JoinGroup", joined);



function joined(group) {

    $("#startchat").css("display", "none");
    $("#chat").css("display", "block");
    $(".inside img").attr("src", `/UploderFiles/${group.picture}`)
    $("#chatname").html(group.groupTitle)
    currentGroupId = group.id;


}
function appendGroup(groupTitle, picture, token) {


    if (groupTitle == null) {
        alert("We could not add this group")
    }
    else {
        $("#chats").show();
        $("#searchbar").hide();
        $("#chats").append(` <a href="#list-chat" class="filterDiscussions all unread single " id="list-chat-list" data-toggle="list" role="tab">
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


