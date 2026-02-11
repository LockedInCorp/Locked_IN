import { Routes, Route } from "react-router-dom"
import { ChatView } from "@/pages/ChatView"
import GroupCreation from "@/pages/GroupCreation"
import GroupEdit from "@/pages/GroupEdit"
import GroupDiscover from "@/pages/GroupDiscover"
import Home from "@/pages/Home"
import Profile from "@/pages/Profile"
import FriendProfile from "@/pages/FriendProfile"
import Register from "@/pages/Register"
import Login from "@/pages/Login"
import Friends from "@/pages/Friends"
import GameProfilesEditingPage from "@/pages/GameProfilesEditing"
import { Header } from "@/components/header/Header"
import { useAuthInit } from "@/hooks/auth/useAuthInit"
import ProtectedRoute from "@/lib/components/ProtectedRoute"

function App() {
    useAuthInit();
    
    return (
        <div className="h-dvh bg-background flex flex-col overflow-hidden">
            <Header />

            <main className="flex flex-1 min-h-0 overflow-hidden">
                <Routes>
                    <Route path="/" element={<Home />} />
                    <Route path="/register" element={<Register />} />
                    <Route path="/login" element={<Login />} />

                    <Route path="/groups" element={<ProtectedRoute><GroupDiscover /></ProtectedRoute>} />
                    <Route path="/my-groups" element={<ProtectedRoute><ChatView /></ProtectedRoute>} />
                    <Route path="/my-groups/:chatId" element={<ProtectedRoute><ChatView /></ProtectedRoute>} />
                    <Route path="/groups/new" element={<ProtectedRoute><GroupCreation /></ProtectedRoute>} />
                    <Route path="/groups/edit/:id" element={<ProtectedRoute><GroupEdit /></ProtectedRoute>} />
                    <Route path="/profile" element={<ProtectedRoute><Profile /></ProtectedRoute>} />
                    <Route path="/profile/game-profiles" element={<ProtectedRoute><GameProfilesEditingPage /></ProtectedRoute>} />
                    <Route path="/friends" element={<ProtectedRoute><Friends /></ProtectedRoute>} />
                    <Route path="/friend/:id" element={<ProtectedRoute><FriendProfile /></ProtectedRoute>} />
                </Routes>
            </main>
        </div>
    )
}

export default App
