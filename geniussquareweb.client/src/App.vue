<script lang="ts">
  import { defineComponent } from 'vue';
  import GameManager from './components/GameLogic/GameManager.vue';
  import MainMenu from './components/MainMenu.vue';
  import * as signalR from "@microsoft/signalr";
  import type { GameData } from './components/GameLogic/GameTypes';

  const States = Object.freeze({
    Loading:   Symbol("Loading"),
    WaitingForSecondPlayer:  Symbol("WaitingForSecondPlayer"),
    MainMenu:  Symbol("MainMenu"),
    Game: Symbol("Game"),
    Modal: Symbol("Modal")
  });

  const GameOutcome = Object.freeze({
    Win:   Symbol("Win"),
    Lose:  Symbol("Lose"),
    Terminated: Symbol("Terminated"),
  });


  export default defineComponent({
      components: {
        GameManager,
        MainMenu
      },
      data(){
          return {
              States,
              state: States.Loading,
              GameOutcome,
              gameOutcome: GameOutcome.Win,
              gameInstanceList : {} as Array<string>,
              currentGame:  null as GameData | null,
              signalRConnection : new signalR.HubConnectionBuilder()
                .withUrl("/gameHub")
                .configureLogging(signalR.LogLevel.Information)
                .withAutomaticReconnect()
                .build() as signalR.HubConnection,
          };
      },
      async created() {
            this.signalRConnection = new signalR.HubConnectionBuilder()
            .withUrl("/gameHub")
              .configureLogging(signalR.LogLevel.Information)
              .build();

            this.signalRConnection.on("ReloadGames", this.fetchData);
            this.signalRConnection.on("LoseGame", this.loseGame);
            this.signalRConnection.on("PlayerJoined", this.startGame);
            this.signalRConnection.on("TerminateGame", this.terminateGame);

            try {
                await this.signalRConnection.start();
                console.assert(this.signalRConnection.state === signalR.HubConnectionState.Connected);
                console.log("SignalR Connected.");
            } catch (err) {
                console.assert(this.signalRConnection.state === signalR.HubConnectionState.Disconnected);
                console.log(err);
                setTimeout(() => this.signalRConnection.start(), 5000);
            }

            console.log(this.signalRConnection.connectionId)

            // fetch the data from server
            await this.fetchData();

            this.state = States.MainMenu;
            this.gameOutcome = GameOutcome.Win;
        },
        async unmounted() {
            await this.signalRConnection.stop();
        },
        methods: {
            async fetchData() {
                this.gameInstanceList = await this.signalRConnection.invoke("GetAllGames")
                .catch(ex =>
                    {
                        console.log(ex);
                    }
                );
            },
            createGame(gameRecord: GameData)
            {
              if (gameRecord == null || this.state !== States.MainMenu)
              {
                return;
              }

              this.currentGame = gameRecord;
              this.state = States.WaitingForSecondPlayer;
            },
            startGame()
            {
              if (this.state !== States.WaitingForSecondPlayer || this.currentGame == null)
              {
                return;
              }

              this.state = States.Game;
            },
            joinGame(gameRecord: GameData)
            {
              if (gameRecord == null || this.state !== States.MainMenu)
              {
                return;
              }

              this.currentGame = gameRecord;
              this.state = States.Game;
            },
            async winGame()
            {
              if (this.state !== States.Game || this.currentGame == null)
              {
                return;
              }

              let didWin: boolean = await this.signalRConnection.invoke("WinGameAsync", this.currentGame.gameGuid)
              .catch(ex =>
                  {
                      console.log(ex);
                  }
              );
            
              if (didWin)
              {
                this.gameOutcome = GameOutcome.Win;
              }
              else
              {
                this.gameOutcome = GameOutcome.Lose;
              }

              this.state = States.Modal;
            },
            loseGame()
            {
              if (this.state !== States.Game)
              {
                return;
              }

              this.gameOutcome = GameOutcome.Lose;
              this.state = States.Modal;
            },
            terminateGame()
            {
              if (this.state !== States.Game || this.state != States.WaitingForSecondPlayer)
              {
                return;
              }

              this.gameOutcome = GameOutcome.Terminated;
              this.state = States.Modal;
            },
            closeWinModal(ev: Event)
            {
              this.state = States.MainMenu;
              this.currentGame = null;
            }
        },
      },
    );
</script>

<template>
    <main>
        <div v-if="state === States.Loading">
          Loading ....
        </div>
        <div v-else-if="state === States.WaitingForSecondPlayer">
          Waiting for another player to join ....
        </div>
        <!-- <MainMenu/> -->
        <div v-else-if="state === States.MainMenu">
          <MainMenu
          :game-instance-list="gameInstanceList"
          @join-game="joinGame"
          @create-game="createGame"
          :signal-r-connection="signalRConnection as signalR.HubConnection">
          </MainMenu>
        </div>

        <!-- <GameManager/> -->      
        <div v-else-if="state === States.Game">
          <GameManager
          @win-game="winGame"
          :current-game="currentGame as GameData">
          </GameManager>
        </div>

        <!-- <Modal/> -->      
        <div v-else-if="state === States.Modal"
          ref="win-modal"
          class="modal">

        <div class="modal-content">
            <span class="close" @click="closeWinModal">&times;</span>
            <h1 v-if="gameOutcome == GameOutcome.Win"  style="background-color: green; color: white;">
                YOU WON
            </h1>
            <h1 v-else-if="gameOutcome == GameOutcome.Lose"  style="background-color: orange; color: white;" >
                YOU LOSE
            </h1>
            <h1 v-else-if="gameOutcome == GameOutcome.Terminated"  style="background-color: blue; color: white;" >
                PLAYER DISCONNECTED. <br/>GAME TERMINATED...
            </h1>
        </div>
    </div>
    </main>
    <br/>
</template>

<style scoped>
  .modal {
    display: block;
    position: fixed;
    z-index: 5;
    padding-top: 100px;
    left: 0;
    top: 0;
    width: 100%;
    height: 100%;
    overflow: auto;
    background-color: rgb(0,0,0); 
    background-color: rgba(0,0,0,0.4);
  }

  /* Modal Content */
  .modal-content {
    background-color: white;
    margin: auto;
    padding: 20px;
    border: 1px solid #888;
    width: 500px;
    text-align: center;
  }

  .close {
    color: red;
    float: right;
    font-size: 28px;
    font-weight: bold;
  }

  .close:hover,
  .close:focus {
    color: darkred;
    text-decoration: none;
    cursor: pointer;
  }

</style>
