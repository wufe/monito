import * as React from 'react';
import './link.scss';
import { ListRowProps } from 'react-virtualized';
import { LinkModel } from '~/types/link';

type Props = LinkModel & ListRowProps;

export const Link = (props: React.PropsWithChildren<Props & ListRowProps>) => {

	return <div className="link__component" style={props.style} key={props.uuid}>
		<div className="__content">
			<div className={`__status-outline --code${props.statusCode}`}></div>
			<div className="__row --header">
				<span></span>
				<span className="__link-id-header"></span>
				<span>URL</span>
				<span className="__status-header">Status</span>
				<span className="__code-header">Code</span>
			</div>
			<div className="__row">
				<span className="__time">{props.updatedAt}</span>
				<span className="__link-id">{props.uuid}</span>
				<span>{props.url}</span>
				<span className="__status">{props.output}</span>
				<span className={`__code --code${props.statusCode}`}>{props.statusCode}</span>
			</div>
		</div>
		
	</div>
}