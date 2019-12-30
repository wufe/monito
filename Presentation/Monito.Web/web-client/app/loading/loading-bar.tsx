import * as React from 'react';
import './loading-bar.scss';

type Props = {
	visible: boolean;
}

export const LoadingBar = (props: React.PropsWithChildren<Props>) =>
	<div className={`loading-bar__component ${props.visible ? '--visible' : ''}`}>
		<div className="__sliding-bar"></div>
	</div>