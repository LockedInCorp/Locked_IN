"use client"

import { useRef, useState } from "react"
import { Upload } from "lucide-react"
import { Link } from "react-router-dom"
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"

type RegisterPart1Props = {
    email: string
    nickname: string
    password: string
    repeatPassword: string
    avatarUrl?: string
    avatarFallback?: string
    onEmailChange: (value: string) => void
    onNicknameChange: (value: string) => void
    onPasswordChange: (value: string) => void
    onRepeatPasswordChange: (value: string) => void
    onAvatarChange: (file: File | null) => void
    onNext: () => void
}

export default function RegisterPart1({
    email,
    nickname,
    password,
    repeatPassword,
    avatarUrl,
    avatarFallback = "U",
    onEmailChange,
    onNicknameChange,
    onPasswordChange,
    onRepeatPasswordChange,
    onAvatarChange,
    onNext
}: RegisterPart1Props) {
    const fileInputRef = useRef<HTMLInputElement>(null)
    const [avatarPreview, setAvatarPreview] = useState<string | null>(null)

    const handleFileSelect = (event: React.ChangeEvent<HTMLInputElement>) => {
        const file = event.target.files?.[0]
        if (file) {
            const reader = new FileReader()
            reader.onloadend = () => {
                const preview = reader.result as string
                setAvatarPreview(preview)
                onAvatarChange(file)
            }
            reader.readAsDataURL(file)
        } else {
            setAvatarPreview(null)
            onAvatarChange(null)
        }
    }

    const handleUploadClick = () => {
        fileInputRef.current?.click()
    }

    const canProceed = email.trim() && nickname.trim() && password.trim() && repeatPassword.trim() && password === repeatPassword

    return (
        <div className="space-y-6">
            {/* Email */}
            <div className="space-y-2">
                <Label htmlFor="email" className="text-sm text-muted-foreground">Email</Label>
                <Input
                    id="email"
                    type="email"
                    value={email}
                    onChange={(e) => onEmailChange(e.target.value)}
                    placeholder="example@abc.com"
                    className="border-border bg-card text-foreground placeholder:text-muted-foreground"
                />
            </div>

            {/* Nickname */}
            <div className="space-y-2">
                <Label htmlFor="nickname" className="text-sm text-muted-foreground">Nickname</Label>
                <Input
                    id="nickname"
                    type="text"
                    value={nickname}
                    onChange={(e) => onNicknameChange(e.target.value)}
                    placeholder="Enter your nickname"
                    className="border-border bg-card text-foreground placeholder:text-muted-foreground"
                />
            </div>

            {/* Password */}
            <div className="space-y-2">
                <Label htmlFor="password" className="text-sm text-muted-foreground">Password</Label>
                <Input
                    id="password"
                    type="password"
                    value={password}
                    onChange={(e) => onPasswordChange(e.target.value)}
                    placeholder="Enter your password"
                    className="border-border bg-card text-foreground placeholder:text-muted-foreground"
                />
            </div>

            {/* Repeat Password */}
            <div className="space-y-2">
                <Label htmlFor="repeat-password" className="text-sm text-muted-foreground">Repeat password</Label>
                <Input
                    id="repeat-password"
                    type="password"
                    value={repeatPassword}
                    onChange={(e) => onRepeatPasswordChange(e.target.value)}
                    placeholder="Repeat your password"
                    className="border-border bg-card text-foreground placeholder:text-muted-foreground"
                />
            </div>

            {/* Profile Picture Upload */}
            <div className="space-y-3">
                <Label className="text-sm text-muted-foreground">Insert your profile picture</Label>
                <div className="flex items-center gap-4">
                    <Avatar className="h-20 w-20">
                        <AvatarImage src={avatarPreview || avatarUrl || "/assets/diverse-user-avatars.png"} />
                        <AvatarFallback>{avatarFallback}</AvatarFallback>
                    </Avatar>
                    <div>
                        <input
                            ref={fileInputRef}
                            type="file"
                            accept="image/*"
                            onChange={handleFileSelect}
                            className="hidden"
                            aria-label="Upload profile picture"
                        />
                        <Button
                            type="button"
                            variant="outline"
                            onClick={handleUploadClick}
                            className="border-border bg-card text-muted-foreground hover:bg-muted hover:text-foreground cursor-pointer"
                        >
                            <Upload className="mr-2 h-4 w-4" />
                            Choose file
                        </Button>
                    </div>
                </div>
            </div>

            {/* Next Button */}
            <div className="flex justify-center pt-4">
                <Button
                    type="button"
                    onClick={onNext}
                    disabled={!canProceed}
                    className="bg-primary px-8 py-2 text-base font-semibold text-primary-foreground hover:bg-primary/90 cursor-pointer disabled:opacity-50 disabled:cursor-not-allowed"
                >
                    Next
                </Button>
            </div>

            {/* Login Link */}
            <div className="text-center pt-2">
                <span className="text-sm text-muted-foreground">
                    If you already have an account please{" "}
                    <Link to="/login" className="text-primary underline hover:text-primary/80">
                        Log In
                    </Link>
                </span>
            </div>
        </div>
    )
}
