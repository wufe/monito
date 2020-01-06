import { ApplicationThunkDispatch } from '~/thunk/thunk';
import { HubConnectionBuilder, HubConnection, HubConnectionState } from '@microsoft/signalr';
import { LinkModel } from '~/types/link';
import { addLinksToCurrentJob } from '~/state/actions/job-actions';
import { store } from '~/state/store';
import { updateLinks } from '~/thunk/job-thunk';

export class JobHub {

	public hubConnection: HubConnection | null;

	constructor(private _dispatch: ApplicationThunkDispatch) {}

	async tryConnect(): Promise<JobHub> {

		if (this.hubConnection && this.hubConnection.state === HubConnectionState.Connected) {
			await this.disconnect();
		}

		this.hubConnection = new HubConnectionBuilder().withUrl('/jobhub').configureLogging("critical").build();
		this.hubConnection.on('RetrieveUpdatedClients', this.RetrieveUpdatedClients.bind(this));

		await this.hubConnection.start();

		return this;
	}

	private RetrieveUpdatedClients(links: LinkModel[]) {
		
		this._dispatch(updateLinks(links))
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