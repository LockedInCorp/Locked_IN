import { Routes, Route } from "react-router-dom"
import { GroupView } from "@/pages/GroupView"
import GroupCreation from "@/pages/GroupCreation"
import GroupEdit from "@/pages/GroupEdit"
import GroupDiscover from "@/pages/GroupDiscover"
import Home from "@/pages/Home"
import Profile from "@/pages/Profile"
import Register from "@/pages/Register"
import Login from "@/pages/Login"
import Friends from "@/pages/Friends"
import { Header } from "@/custom_components/header/Header"
import { useAuthInit } from "@/hooks/auth/useAuthInit"

function App() {
    useAuthInit();
    
    return (
        <div className="h-dvh bg-background flex flex-col overflow-hidden">
            <Header />

            <main className="flex flex-1 min-h-0 overflow-hidden">
                <Routes>
                    <Route path="/" element={<Home />} />
                    <Route path="/my-groups" element={<GroupView />} />
                    <Route path="/my-groups/:chatId" element={<GroupView />} />
                    <Route path="/groups" element={<GroupDiscover />} />
                    <Route path="/groups/new" element={<GroupCreation />} />
                    <Route path="/groups/edit/:id" element={<GroupEdit />} />
                    <Route path="/profile" element={<Profile />} />
                    <Route path="/friends" element={<Friends />} />
                    <Route path="/register" element={<Register />} />
                    <Route path="/login" element={<Login />} />
                </Routes>
            </main>
        </div>
    )
}

export default App
