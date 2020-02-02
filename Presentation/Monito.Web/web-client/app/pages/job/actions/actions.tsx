import * as React from 'react';
import './actions.scss';

type Props = {
	showTruncatedMessage: boolean;
	showAvatar: boolean;
	downloadCSV?: boolean;
	isDownloadingCSV?: boolean;
	onDownloadCSVClick?: () => any;
	downloadJSON?: boolean;
	isDownloadingJSON?: boolean;
	onDownloadJSONClick?: () => any;
	abortJob?: boolean;
	isAbortingJob?: boolean;
	onAbortJobClick?: () => any;
}

export const Actions = (props: React.PropsWithChildren<Props>) => <div className="actions__component">
	<div className="__content">
		<div className="__messages">
			{props.showTruncatedMessage ? <div className="__message --info">
				<span>Results listed here have been <b>truncated</b> to <b>2000</b> due to performance issues.</span>
				<span>You can however download the entire report.</span>
			</div> : null}
			{props.showAvatar && !props.showTruncatedMessage ? <div className="__avatar-container">
				<img src="https://www.habbo.it/habbo-imaging/avatar/hd-180-14.ch-235-1408.lg-270-1408.sh-300-1408.ha-1015.he-1609-1408.fa-1201%2Cs-4.g-1.d-2.h-2.a-0%2C09ec8fb6f75fea13b30dbf4a8336f4e3.png"/>
				<div className="__speech-bubble">Your report is ready!</div>
			</div> : null}
		</div>
		<div className="__actions">
			{props.downloadCSV ? <div className={`__button-container ${props.isDownloadingCSV ? '--open' : ''}`}>
				<div className="__button --dark" onClick={props.onDownloadCSVClick || (e => {})}>Download CSV</div>
				<div className="__description">Your download is starting..</div>
			</div> : null}
			{props.downloadJSON ? <div className={`__button-container ${props.isDownloadingJSON ? '--open' : ''}`}>
				<div className="__button --dark" onClick={props.onDownloadJSONClick || (e => {})}>Download JSON</div>
				<div className="__description">Currently not available</div>
			</div> : null}
			{props.abortJob ? <div className={`__button-container`}>
				<div className={`__button --danger ${props.isAbortingJob ? '--disabled' : ''}`} onClick={props.onAbortJobClick || (e => {})}>Abort</div>
			</div> : null}
		</div>
	</div>
</div>