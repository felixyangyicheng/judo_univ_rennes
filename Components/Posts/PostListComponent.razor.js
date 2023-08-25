
export function scrollToElement(id) {
   

    var element = document.getElementById(id);
    console.log(element.lastChild.previousSibling);

    //element.lastChild.scrollIntoView();
    //console.log("step 1")
    
    //element.scrollIntoView(false);
    //console.log("step 2")

    //element.scrollIntoView({ block: "end" });
    //console.log("step 3")

    element.lastChild.previousSibling.scrollIntoView({ behavior: "smooth", block: "end", inline: "nearest" });


}
