import * as React from 'react';
import './link.scss';
import { ListRowProps } from 'react-virtualized';
import { LinkModel } from '~/types/link';
import moment from 'moment';

type Props = LinkModel & ListRowProps;

export const Link = (props: React.PropsWithChildren<Props & ListRowProps>) => {

	const code = props.statusCode;

	let codeClass = 'xxx';
	if (code >= 200 && code < 300) {
		if (code === 200) {
			codeClass = `${code}`;
		} else {
			codeClass = '2xx';
		}
	} else if (code >= 300 && code < 400) {
		if (code === 301 || code === 302 || code === 307 || code === 308) {
			codeClass = `${code}`;
		} else {
			codeClass = '3xx';
		}
	} else if (code >= 400 && code < 500) {
		if (code === 400 || code === 401 || code === 403 || code === 404 || code === 408 || code === 429) {
			codeClass = `${code}`;
		} else {
			codeClass = '4xx';
		}
	} else if (code >= 500) {
		if (code === 500 || code === 502 || code === 503 || code === 504) {
			codeClass = `${code}`;
		} else {
			codeClass = '5xx';
		}
	}

	const time = moment(props.updatedAt);

	return <div className="link__component" style={props.style} key={props.uuid}>
		<div className="__content">
			<div className={`__status-outline --code${codeClass}`}></div>
			<div className="__row --header">
				<span></span>
				<span className="__link-id-header"></span>
				<span>URL</span>
				<span className="__status-header">Status</span>
				<span className="__code-header">Code</span>
			</div>
			<div className="__row">
				<span className="__time" title={time.format("MMM YYYY hh:MM:ss")}>{time.format('hh:MM:ss')}</span>
				<span className="__link-id">{props.uuid}</span>
				<span>{props.url}</span>
				<span className="__status" title={props.output}>{props.output}</span>
				<span className={`__code --code${codeClass}`}>{props.statusCode}</span>
			</div>
		</div>
		
	</div>
}