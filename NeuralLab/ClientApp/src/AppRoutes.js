import Project from './components/Project';

function AppRoutes(userid) {
    return ([
        {
            index: true,
            element: <Project userid={userid} />
        },
    ]);
}

export default AppRoutes;