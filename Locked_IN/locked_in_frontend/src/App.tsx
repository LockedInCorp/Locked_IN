import { Routes, Route } from 'react-router-dom'
import { GroupView } from './pages/GroupView'

function App() {
    return (
        <div className="min-h-screen bg-background">
            <Routes>
                {/*<Route path="/" element={<Navigate to="/groups" replace />} />*/}
                <Route path="/" element={<GroupView />} />
            </Routes>
        </div>
    )
}

export default App