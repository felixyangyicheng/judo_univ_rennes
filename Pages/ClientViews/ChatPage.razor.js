﻿
export function scrollToElement(id) {
   

    var element = document.getElementById(id);
    console.log(element.lastChild);

    //element.lastChild.scrollIntoView();
    //console.log("step 1")
    
    //element.scrollIntoView(false);
    //console.log("step 2")

    //element.scrollIntoView({ block: "end" });
    //console.log("step 3")

    element.lastChild.scrollIntoView({ behavior: "smooth", block: "end", inline: "nearest" });
    console.log("step 4")

   // element.scroll(0,0);


}
export function testConsole(id) {
    console.log(id);
}