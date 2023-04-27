






export function initGoogle  (dotNetObjectRef, clientid) {

    function handleCredentialResponse   (response) {

        console.log("Encoded JWT ID token: " + response.credential);

        dotNetObjectRef.invokeMethodAsync('SaveCredentials', response.credential);
    }




    google.accounts.id.initialize({
        client_id: clientid,
        callback: handleCredentialResponse
    });
    google.accounts.id.renderButton(
        document.getElementById("buttonDiv"),
        { theme: "outline", size: "large" }  // customization attributes
    );
    //  google.accounts.id.prompt(); // also display the One Tap dialog
}


