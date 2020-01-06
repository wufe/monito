import * as React from 'react';
import './actions.scss';

type Props = {
	showTruncatedMessage: boolean;
	showAvatar: boolean;
	isDownloadingCSV: boolean;
	isDownloadingJSON: boolean;
	onDownloadCSVClick: () => any;
	onDownloadJSONClick: () => any;
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
			<div className={`__button-container ${props.isDownloadingCSV ? '--open' : ''}`}>
				<div className="__button --dark" onClick={props.onDownloadCSVClick}>Download CSV</div>
				<div className="__description">Your download is starting..</div>
			</div>
			<div className={`__button-container ${props.isDownloadingJSON ? '--open' : ''}`}>
				<div className="__button --dark" onClick={props.onDownloadJSONClick}>Download JSON</div>
				<div className="__description">Currently not available</div>
			</div>
		</div>
	</div>
</div>