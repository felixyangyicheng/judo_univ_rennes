
export function scrollToElement(id) {
   

    var element = document.getElementById(id);

    element.lastChild.scrollIntoView();
    element.scrollIntoView(false);
    element.scrollIntoView({ block: "end" });
    element.lastChild.scrollIntoView({ behavior: "smooth", block: "end", inline: "nearest" });



}
export function testConsole(id) {
    console.log(id);
}