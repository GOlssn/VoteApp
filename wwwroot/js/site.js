function addField() {
    var field = document.createElement("input");
    field.type = "text";
    field.name = "Options";
    field.className = "form-control";

    document.getElementById("poll-options").appendChild(field);
}

document.getElementById("moreOptions").addEventListener("click", function (event) {
    event.preventDefault();
    addField();
});