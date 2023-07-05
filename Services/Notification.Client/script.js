import {showMessage} from "./notification.js";
import {tryConnect} from "./builder.js";

export const main = document.querySelector('body');
export const content = document.getElementById('content');
const connection = await tryConnect().catch((er) => console.log(er));

connection.onclose(async () => {
    main.style.border = '1px solid #FF5722'
    showMessage('Connection closed by server', 'danger', 'border-danger')
    await tryConnect().then(c=>{
    showMessage('Reconnecting', 'success', 'border-success')
    });
})
connection.on('Initialized', (msg) => showMessage(msg, 'success', 'border-success'));

connection.on('all', (msg) =>
    document.body.appendChild(pCreator(JSON.stringify(msg), 'message', 'content')));

function pCreator(innerText, classname = null, id = null) {
    const element = document.createElement('p');
    element.setAttribute('class', classname);
    element.setAttribute('id', id);
    element.innerText = innerText;
    return element
}