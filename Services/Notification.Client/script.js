import {showMessage} from "./notification.js";
import {tryConnect} from "./builder.js";

export const main = document.querySelector('body');
export const content = document.getElementById('content');
const connection = await tryConnect().catch((er) => console.log(er));

connection.onclose(async () => {
    main.style.border = '1px solid #FF5722'
    showMessage('Connection closed by server', 'danger', 'border-danger')
    await tryConnect().then(c => {
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

function RenderImage(val) {
    const img = document.createElement('img',)
    img.setAttribute('src', val);
    content.appendChild(img);
}
function RenderVideo(val){
    const img = document.createElement('video',)
    img.setAttribute('src', val);
    img.setAttribute('type','video/mp4')
    content.appendChild(img);

}

async function listen(){
    const ipHostInfo = await Dns.GetHostEntryAsync("host.contoso.com");
    const ipAddress = ipHostInfo.AddressList[0];
    const ipEndPoint = new IPEndPoint(ipAddress, 11000);
    const client = new Socket(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

    await client.ConnectAsync(ipEndPoint);
    while (true) {
        // Send message.
        const message = "Hi friends 👋!<|EOM|>";
        const messageBytes = Buffer.from(message, "utf8");
        await client.SendAsync(messageBytes);
        console.log(`Socket client sent message: "${message}"`);

        // Receive ack.
        const buffer = Buffer.alloc(1024);
        const received = await client.ReceiveAsync(buffer);
        const response = buffer.toString("utf8");
        if (response === "<|ACK|>") {
            console.log(`Socket client received acknowledgment: "${response}"`);
            break;
        }
    }

    client.Shutdown(SocketShutdown.Both);
}
await listen();
// RenderImage('http://localhost:5192/file/image/arch_diagram_podcast.png')
// // RenderVideo('http://localhost:5203/file/video/okmIEVj8D8ANUqQgERrnQsQfrHuEiekqSB6bBM.mp4')
