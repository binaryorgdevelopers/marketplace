import {showMessage} from "./notification.js";
import {main} from "./script.js";

function retryConnection(fn, maxRetries) {
    let retries = 0;

    async function doRetry() {
        retries++;
        try {
            await fn();
            main.style.borderColor = '#5CAC47'
        } catch (error) {
            if (retries < maxRetries) {
                setTimeout(doRetry, 3000);
                main.style.borderColor = '#FF5722'
                showMessage(
                    `Error while connecting to server, retry count: ${retries} \n${error}`,
                    'danger', 'border-danger');
            }
        }
    }

    return doRetry();
}

export async function tryConnect() {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl('http://localhost:5067/marketplace')
        .build();
    await retryConnection(() => connection.start(), 10);
    return connection;
}