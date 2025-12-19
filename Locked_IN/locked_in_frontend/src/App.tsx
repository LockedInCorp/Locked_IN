import { Routes, Route } from "react-router-dom"
import { GroupView } from "@/pages/GroupView"
import GroupCreation from "@/pages/GroupCreation"
import GroupDiscover from "@/pages/GroupDiscover"
import Home from "@/pages/Home"
import Profile from "@/pages/Profile"
import Register from "@/pages/Register"
import Login from "@/pages/Login"
import { Header } from "@/custom_components/header/Header"

function App() {
    return (
        <div className="h-dvh bg-background flex flex-col overflow-hidden">
            <Header />

            <main className="flex flex-1 min-h-0 overflow-hidden">
                <Routes>
                    <Route path="/" element={<Home />} />
                    <Route path="/my-groups" element={<GroupView />} />
                    <Route path="/groups" element={<GroupDiscover />} />
                    <Route path="/groups/new" element={<GroupCreation />} />
                    <Route path="/profile" element={<Profile />} />
                    <Route path="/register" element={<Register />} />
                    <Route path="/login" element={<Login />} />
                </Routes>
            </main>
        </div>
    )
}

export default App
