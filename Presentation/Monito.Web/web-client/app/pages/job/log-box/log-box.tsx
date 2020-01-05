import * as React from 'react';
import './log-box.scss';
import { Scrollbars } from 'react-custom-scrollbars';

type Props = {
	messages: string[];
}

export const LogBox = (props: React.PropsWithChildren<Props>) => {

	const scrollableContent = React.useRef<Scrollbars>(null);

	React.useEffect(() => {
		let scrollableDiv = scrollableContent.current;
		if (scrollableDiv) {
			scrollableDiv.scrollToBottom();
		}
	})

	return <div className="log-box__component">
		<div className="__content">
			<Scrollbars
				ref={scrollableContent}
				renderThumbVertical={props => <div {...props} className="__vertical-thumb"></div>}
				style={{
					width: "100%",
					height: 140
				}}>
				{props.messages.map((message, index) => <div className="__message" key={index}>
					{message}
				</div>)}
			</Scrollbars>
		</div>
		<div className="__overlay"></div>
	</div>
};