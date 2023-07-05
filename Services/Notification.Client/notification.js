import {content} from "./script.js";

export function showMessage(message, badge, border) {
    const messageBox = document.createElement('div');
    messageBox.classList.add('message_box');
    messageBox.classList.add(border);

    const h3 = document.createElement('h3');
    h3.setAttribute('id', 'message-text');
    h3.innerText = message;
    h3.classList.add(badge)
    messageBox.appendChild(h3);

    content.append(messageBox)

    setTimeout(() => removeMessage(), 3000);

    function removeMessage() {
        if (content.contains(messageBox)) {
            content.removeChild(messageBox)
        }
    }
}