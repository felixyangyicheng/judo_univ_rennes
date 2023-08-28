
export function scrollToElement(id) {
   

    var element = document.getElementById(id);


    element.lastChild.scrollIntoView({ behavior: "smooth", block: "end", inline: "nearest" });

}
export function testConsole(id) {
    console.log(id);
}