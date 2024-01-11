

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();



connection.on("SendClientMessage", function () {
    console.log("succses");
})
connection.on("NewGroup", appendGroup);

function appendGroup(groupTitle, picture, token) {


    if (groupTitle == null) {
        alert("We could not add this group")
    }
    else {

        $("#chats").append(` <a href="#list-chat" class="filterDiscussions all unread single active" id="list-chat-list" data-toggle="list" role="tab">
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
                $("#searchbar").append(` <a href="#list-chat" class="filterDiscussions all unread single active" id="list-chat-list" data-toggle="list" role="tab">
                                <img class="avatar-md" src="/UploderFiles/${data[i].picture}" data-toggle="tooltip" data-placement="top" title="Janette" alt="avatar">
                                <div class="data">
                                    <h5>${data[i].title}</h5>
                                 
                                </div>
                            </a>`)
            }


        });
    } else {
        $("#chats").show();
        $("#searchbar").hide();
    }
}