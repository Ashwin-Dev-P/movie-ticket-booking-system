// To toggle show more or less of description of the movies
function expandDescription(anchorElement, id) {
    document.getElementById(id).classList.toggle('expand');

    var innerText = anchorElement.innerHTML;

    if (innerText == 'Show more...') {
        anchorElement.innerHTML = "Show less";
    } else {
        anchorElement.innerHTML = "Show more...";
    }

}