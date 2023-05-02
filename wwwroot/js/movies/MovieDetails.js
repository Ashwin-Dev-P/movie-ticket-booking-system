
const initailDescriptionHeight = document.getElementById("description").clientHeight;
console.info("Height of the description is:", initailDescriptionHeight);
if (initailDescriptionHeight  < 100) {
    

    document.getElementById("descriptionHeightToggler").remove();
}


// To toggle show more or less of description of the movies
function expandDescription(anchorElement) {
    document.getElementById("description").classList.toggle('expand');

    var innerText = anchorElement.innerHTML;

    if (innerText == "Show more...") {
        anchorElement.innerHTML = "Show less";
    } else if (innerText == "Show less") {
        anchorElement.innerHTML = "Show more...";
    } else {
        console.error("Error");
    }

}