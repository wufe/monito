import * as React from 'react';
import { List, ListRowProps, WindowScroller, AutoSizer } from 'react-virtualized';
import './links-box.scss';
import { Link } from './link';
import { useSelector } from 'react-redux';
import { GlobalState } from '~/state/state';
import { LinkModel } from '~/types/link';

export const LinksBox = () => {

	let links = useSelector<GlobalState, LinkModel[]>(x => {
		if (!x.job.job)
			return [];
		return x.job.job.links;
	});
		
	const LinkRowRenderer = (props: ListRowProps) => {
		const link = links[props.index];
		return <Link {...props} {...link} />;
	}

	return <div className="links-box__component">
		<div className="__content">
			<AutoSizer disableHeight={true}>
				{({width}) => 
					<WindowScroller>
						{({height, isScrolling, onChildScroll, scrollTop}) =>
							<List
								autoHeight
								width={width}
								height={height}
								isScrolling={isScrolling}
								onScroll={onChildScroll}
								scrollTop={scrollTop}
								rowCount={links.length}
								rowHeight={70}
								rowRenderer={LinkRowRenderer} />}
					</WindowScroller>
				}
			</AutoSizer>
		</div>
	</div>

}