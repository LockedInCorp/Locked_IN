# Chat Functionality Testing Guide

This guide will help you test the chat functionality in your application.

## Prerequisites

1. **Database Migration**: Ensure you've run the migration to add the new chat fields:
   ```bash
   cd Locked_IN_Backend
   dotnet ef database update
   ```

2. **Backend Running**: Start your backend API:
   ```bash
   dotnet run
   ```
   The API should be available at `http://localhost:5122`

3. **Database Setup**: Make sure you have at least 2 users in your database (IDs 1 and 2) for testing.

## Testing Methods

### Method 1: Using Swagger UI (Easiest)

1. Navigate to `http://localhost:5122/swagger` in your browser
2. Find the `Chat` controller endpoints
3. Test endpoints in this order:

   **Step 1: Create a Chat**
   - Use `POST /api/chat`
   - Set `userId` query parameter: `1`
   - Body example:
     ```json
     {
       "type": "Direct",
       "name": null,
       "teamId": null,
       "participantUserIds": [1, 2]
     }
     ```
   - Copy the returned `chatId` for next steps

   **Step 2: Send a Message**
   - Use `POST /api/chat/messages`
   - Set `userId` query parameter: `1`
   - Body example:
     ```json
     {
       "chatId": 1,
       "content": "Hello! This is a test message.",
       "attachmentUrl": null
     }
     ```

   **Step 3: Get Messages**
   - Use `GET /api/chat/{chatId}/messages`
   - Set `chatId` to the ID from Step 1
   - Set `userId` query parameter: `1`

   **Step 4: Get User Chats**
   - Use `GET /api/chat/user/{userId}`
   - Set `userId` to `1`

### Method 2: Using HTTP File (REST Client)

1. Open `ChatTests.http` in Visual Studio Code or Rider
2. Update the variables at the top:
   - `@userId1` - First test user ID
   - `@userId2` - Second test user ID
   - `@chatId` - Will be set after creating a chat
3. Run requests in order:
   - Create a chat first
   - Copy the chat ID and update `@chatId`
   - Send messages
   - Test other endpoints

### Method 3: Using SignalR Test Client (Real-time Testing)

1. Open `TestSignalR.html` in a web browser
2. **Setup:**
   - Hub URL: `http://localhost:5122/chathub` (should be pre-filled)
   - Chat ID: Enter the chat ID you created
   - User ID: Enter your user ID (1 or 2)

3. **Test Real-time Messaging:**
   - Click "Connect" to establish SignalR connection
   - Click "Join Chat Group" to join the chat room
   - Open **another browser tab/window** with the same page
   - Use different User IDs in each tab
   - Send messages from one tab - they should appear in real-time in the other tab

4. **Test SignalR Events:**
   - Send a message via the input field (uses REST API + SignalR broadcast)
   - Watch the Event Log for SignalR events
   - Test editing/deleting messages from Swagger/HTTP file and watch SignalR events

### Method 4: Using Postman or cURL

#### Create Chat:
```bash
curl -X POST "http://localhost:5122/api/chat?userId=1" \
  -H "Content-Type: application/json" \
  -d '{
    "type": "Direct",
    "name": null,
    "teamId": null,
    "participantUserIds": [1, 2]
  }'
```

#### Send Message:
```bash
curl -X POST "http://localhost:5122/api/chat/messages?userId=1" \
  -H "Content-Type: application/json" \
  -d '{
    "chatId": 1,
    "content": "Hello from cURL!",
    "attachmentUrl": null
  }'
```

#### Get Messages:
```bash
curl "http://localhost:5122/api/chat/1/messages?userId=1&pageNumber=1&pageSize=50"
```

## Testing Scenarios

### Scenario 1: Direct Chat Flow
1. User 1 creates a direct chat with User 2
2. User 1 sends a message
3. User 2 receives the message (check via SignalR or API)
4. User 2 marks chat as read
5. User 2 sends a reply
6. Verify unread counts update correctly

### Scenario 2: Group Chat Flow
1. User 1 creates a group chat with Users 2 and 3
2. All users join the SignalR group
3. User 1 sends a message
4. Verify Users 2 and 3 receive it in real-time
5. User 2 sends a message
6. Verify Users 1 and 3 receive it

### Scenario 3: Message Editing
1. User 1 sends a message
2. User 1 edits the message
3. Verify the edited message appears for all participants
4. Verify `editedAt` timestamp is set

### Scenario 4: Message Deletion
1. User 1 sends a message
2. User 1 deletes the message
3. Verify the message is soft-deleted (not shown in GetMessages)
4. Verify SignalR broadcasts deletion event

### Scenario 5: Read Receipts
1. User 1 sends a message
2. User 2 marks chat as read
3. Verify `lastReadAt` and `unreadCount` update
4. Verify SignalR broadcasts read receipt

## Expected Behavior

### ✅ Success Indicators:
- Messages persist in database
- Messages broadcast via SignalR in real-time
- Unread counts increment correctly
- Read receipts update properly
- Chat list shows last message and unread count
- Pagination works for message history

### ❌ Error Cases to Test:
- Sending message to chat you're not in → Should return error
- Editing someone else's message → Should return error
- Getting chat you're not a participant in → Should return error
- Creating direct chat that already exists → Should return existing chat

## Debugging Tips

1. **Check Database:**
   ```sql
   SELECT * FROM chat;
   SELECT * FROM message;
   SELECT * FROM chatparticipant;
   ```

2. **Check SignalR Connection:**
   - Open browser DevTools → Network tab
   - Look for WebSocket connection to `/chathub`
   - Check for any connection errors

3. **Check Backend Logs:**
   - Watch console output for errors
   - Check for database connection issues
   - Verify CORS is configured correctly

4. **Common Issues:**
   - **CORS Error**: Make sure CORS policy allows your frontend origin
   - **SignalR Not Connecting**: Check hub URL and CORS settings
   - **Messages Not Persisting**: Check database connection string
   - **Unread Count Not Updating**: Verify participant exists in chat

## Next Steps

Once basic functionality works:
1. Test with your frontend application
2. Add authentication/authorization
3. Test with multiple concurrent users
4. Test file attachments (when implemented)
5. Test with high message volume

## API Endpoints Summary

| Method | Endpoint | Purpose |
|--------|----------|---------|
| POST | `/api/chat` | Create chat |
| POST | `/api/chat/messages` | Send message |
| GET | `/api/chat/{chatId}/messages` | Get messages |
| GET | `/api/chat/user/{userId}` | Get user's chats |
| GET | `/api/chat/{chatId}` | Get chat details |
| PUT | `/api/chat/messages` | Edit message |
| DELETE | `/api/chat/messages/{messageId}` | Delete message |
| POST | `/api/chat/{chatId}/read` | Mark as read |
| POST | `/api/chat/{chatId}/join` | Join chat |
| POST | `/api/chat/{chatId}/leave` | Leave chat |

## SignalR Hub Endpoint

- **URL**: `http://localhost:5122/chathub`
- **Methods**:
  - `JoinChatGroup(chatId)` - Join a chat room
  - `LeaveChatGroup(chatId)` - Leave a chat room
- **Events**:
  - `ReceiveMessage` - New message received
  - `MessageEdited` - Message was edited
  - `MessageDeleted` - Message was deleted
  - `MessageRead` - Read receipt received

