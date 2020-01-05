import { ApplicationThunkDispatch } from '~/thunk/thunk';
import { HubConnectionBuilder, HubConnection, HubConnectionState } from '@microsoft/signalr';

export class JobHub {

	public hubConnection: HubConnection | null;

	constructor(private _dispatch: ApplicationThunkDispatch) {}

	async tryConnect(): Promise<JobHub> {
		if (this.hubConnection && this.hubConnection.state === HubConnectionState.Connected)
			return this;

		this.hubConnection = new HubConnectionBuilder().withUrl('/jobhub').build();
		this.hubConnection.on('RetrieveUpdatedClients', this.RetrieveUpdatedClients);
		this.hubConnection.on('Error', console.log);

		await this.hubConnection.start();

		return this;
	}

	private RetrieveUpdatedClients() {
		console.log(arguments);
	}

	disconnect() {

		if (!this.hubConnection)
			return;

		// fire and forget
		this.hubConnection.stop();

		this.hubConnection = null;
		return;
	}

}

class JobHubInstanceHolder {

	constructor(private _hubInstance: JobHub | null) {}

	instance(builder: () => JobHub) {
		if (this._hubInstance)
			return this._hubInstance;
		this._hubInstance = builder();
		return this._hubInstance;
	}

	register(hubInstance: JobHub) {
		this._hubInstance = hubInstance;
		return this;
	}

	disconnect() {
		if (!this._hubInstance)
			return;
		this._hubInstance.disconnect();
		this._hubInstance = null;
		return this;
	}
}

export const jobHubInstanceGetter = new JobHubInstanceHolder(null);