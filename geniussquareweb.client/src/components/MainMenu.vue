<template>
    <div>
        <h1 style="color: darkgreen;"> Join existing game</h1>
        <ul>
            <li v-for="(gameId, index) in gameInstanceList"> 
                <button 
                    class="link"
                    @click="joinGameRoom(gameId)">
                    {{gameId}}
                </button>
            </li>
        </ul>
        <button
            class="playerButton"
            @click="createGameRoom">
            New game (Player)
        </button>
    </div>
    <br/>
    <div style="width:800px;">
        <button 
            class="botButton easyBotButton"
            @click="createGameRoomBot(0)">
            New game (Easy Bot)
        </button>
        <button 
            class="botButton mediumBotButton"
            @click="createGameRoomBot(1)">
            New game (Medium Bot)
        </button>
        <button 
            class="botButton hardBotButton"
            @click="createGameRoomBot(2)">
            New game (Hard Bot)
        </button>
    </div>
</template>

<script lang="ts">
    import * as signalR from "@microsoft/signalr";
    import { defineComponent, type PropType } from 'vue';
    import type { GameData } from "./GameLogic/GameTypes";

    export default defineComponent({
        emits: ["createGame", "joinGame"],
        props: {
            gameInstanceList : Array<string>,
            signalRConnection :
            {
                type: Object as PropType<signalR.HubConnection>,
                required: true,

            },
        },
        methods: {
                async createGameRoom()
                {
                    if (this.signalRConnection == undefined)
                    {
                        throw new Error('SignalR connection should not be null');
                    }

                    let gameRecord:GameData = await this.signalRConnection.invoke("CreateGameAsync")
                    .catch(ex =>
                    {
                        console.log(ex);
                    });

                    if(gameRecord == null)
                    {
                        alert("Failed to create game");
                        return;
                    }
                    
                    this.$emit("createGame", gameRecord);

                },
                async createGameRoomBot(type:number)
                {
                    if (this.signalRConnection == undefined)
                    {
                        throw new Error('SignalR connection should not be null');
                    }

                    let gameRecord:GameData = await this.signalRConnection.invoke("CreateGameBotAsync",type)
                    .catch(ex =>
                    {
                        console.log(ex);
                    });

                    if(gameRecord == null)
                    {
                        alert("Failed to create game");
                        return;
                    }
                    
                    this.$emit("joinGame", gameRecord);
                },
                async joinGameRoom(gameId: string)
                {

                    let gameRecord:GameData = await this.signalRConnection.invoke("JoinGameAsync", gameId)
                    .catch(ex =>
                    {
                        console.log(ex);
                    });

                    if(gameRecord == null)
                    {
                        alert("Game is full. Try another one.");
                        return;
                    }
                                   
                    this.$emit("joinGame", gameRecord);
                }
            },
        },
    );
</script>

<style scoped>

ul{height:400px; width:400px;}
ul{overflow:hidden; overflow-y:scroll;}

.playerButton {
    background-color: green;
    color: white;
    border: none;
    padding: 10px 20px;
    font-size: 16px;
    cursor: pointer;
    border-radius: 4px;
    transition: background-color 0.3s ease;
}

.botButton
{
    color: white;
    border: none;
    padding: 10px 20px;
    font-size: 16px;
    cursor: pointer;
    border-radius: 4px;
    transition: background-color 0.3s ease;
    margin-right: 10px;
}

.easyBotButton {
    background-color: orange;
}

.mediumBotButton {
    background-color: rgb(255, 120, 0);
}

.hardBotButton {
    background-color: rgb(255, 100, 0);
}

/* Hover state */
.playerButton:hover {
    background-color: darkgreen;
}

/* Hover state */
.easyBotButton:hover {
    background-color: rgb(187, 106, 8);
}

/* Hover state */
.mediumBotButton:hover {
    background-color: rgb(187, 106, 8);
}

/* Hover state */
.hardBotButton:hover {
    background-color: rgb(187, 106, 8);
}

button.link { 
    background:none;
    border:none;
    color: grey;
}

button.link:hover{
    color: red;
}
</style>