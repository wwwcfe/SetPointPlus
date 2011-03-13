SetPointPlus 1.0.0.1
公開日 2011/01/15
Web : http://d.hatena.ne.jp/wwwcfe/20090901/SetPointPlus
Mail: wwwcfe@live.com
ライセンス: The MIT License
(License.txt を参照。参考に日本語訳 License.ja.txt を同梱しています。)

* 注意
  動作には .NET Framework 2.0 以上が必要です。
  現在、SetPoint 5.x 系には対応していません。
  SetPoint 4.x 系と SetPoint 6.x 系に対応しています。

* 概要
  標準の SetPoint で設定できない項目をマウスやキーボードに設定できるように
  します。一度設定した後は削除してかまいません。

* 使い方
  実行には管理者権限が必要です。
  オプション一覧:
    -v6 : SetPoint 6.x を対象にします。SetPointPlus を適用するには -v6
          または -v4 のいずれかを指定する必要があります。
    -v4 : SetPoint 4.x を対象にします
    -a  : マウスの左右ボタンにすべての利用可能なコマンドを表示します。
          UAC ウィンドウなどでマウスが効かなくなる問題があります。
          デフォルトでは無効です。
    -r  : SetPointPlus で書き換えたファイルを元に戻します。
    -l  : インストールされているデバイスを表示します。
          ここで表示されたデバイスの ID (100009F など) を引数で指定した場合、
          そのデバイスのみに SetPointPlus を適用できます。
  使い方の例:
    まず、コマンドプロンプトを管理者権限で実行します。
    そして、コマンドプロンプトで SetPointPlus を展開したフォルダに移動します。
    > cd C:\SetPointPlus\
    次に、オプションを参考にしながら以下のように SetPointPlus を実行します。
    > SetPointPlus.exe -v6
    > SetPointPlus.exe -v4
    > SetPointPlus.exe -v6 -r
    > SetPointPlus.exe -v6 -l
    > SetPointPlus.exe -v6 100009F
    > SetPointPlus.exe -v6 -a

* バックアップ
  適用時にバックアップが自動生成されます。初回のみバックアップします。
  以下にバックアップファイルを示します。
  SetPoint 4.x
  ・"%ProgramFiles%\Logicool\SetPoint\default.xml.bak"
  ・"%ProgramData%\Logitech\SetPoint\Devices\{デバイスの種類}\{デバイスID}\
    {デバイスID}.xml.bak"
  ・"%AppData%\Logitech\SetPoint\user.xml.bak"
  SetPoint 6.x
  ・"%ProgramFiles%\Logicool\SetPointP\default.xml.bak"
  ・"%ProgramData%\Logishrd\SetPointP\Devices\{デバイスの種類}\{デバイスID}\
    {デバイスID}.xml.bak"
  ・"%AppData%\Logitech\SetPoint\user.xml.bak"

* 国際化について
  SetPointPlus 1.0 から GUI の削除に伴い、簡略化のため日本語言語ファイルを
  削除しました。プログラムは英語のみ対応しています。使い方が分からなくなった
  場合はこのファイルを参照してください。

* 既知の問題
  ・-a オプションで適用した場合 UAC ダイアログでマウスの左右ボタンが効きません。
    これは、汎用ボタンを割り当てると動作します。他にキーボードで操作する方法も
    あります。
  ・SetPoint を閉じるボタン [X] で閉じるとフリーズすることがあります。
    もしこの問題が起こる場合は、閉じるボタンは使わず、キャンセル ボタンを
    利用するか、マウスの左クリックに「汎用ボタン」を割り当ててください。
  ・Kaspersky と競合することがあります。
    SetPoint.exe, SetPoint32.exe. SetPointPlus.exe を除外リストに追加して
    ください。
  ・「TripleKeystroke」を割り当てた場合、3 番目のキーストロークを設定できません。
    user.xml を手動で書き換えることで設定できますが、理解できる方のみご利用ください。

* 更新履歴
  2011/01/15 Version 1.0.0.1
  ・SetPoint の再起動時に SetPointPlus 自身も終了させてしまっていた問題を修正
    (この問題のため、SetPoint を再起動できなくなっていた)
  ・利便性のため v4.bat (SetPoint 4.x 向け)、v6.bat (SetPoint 6.x 向け) を追加
    SetPointPlus を適用するときはどちらかを管理者権限で実行してください。
    エクスプローラーの右クリックから「管理者権限として実行」が便利です。
  ・DoubleKeystroke と TripleKeystroke の初期設定で 0 が表示されていたのを
    表示されないように修正
  2011/01/10 Version 1.0.0.0
  ・このバージョンから MIT License を適用します。
  ・GUI を削除し、コンソールアプリケーションになりました。
  ・簡略化のために日本語言語ファイルを削除しました。
    使い方が分からなくなった場合は SetPointPlus.txt を参照してください。
  ・ソースコードを公開しました。
    https://gist.github.com/772800
  ・.NET Framework 2.0 で動作するようになりました。
  ・機能的には SetPointPlus 0.1.0.9 とほぼ同等です。GUI がほしい場合は
    0.1.0.9 を利用してください。ただし GUI のコードは今後保守しません。
  2010/10/05 Version 0.1.0.9
  ・連続してキーストロークを送信する「TripleKeystroke」と「DoubleKeystroke」
    を追加
  ・起動時にインストールされている SetPoint のバージョンを自動検出
  2010/08/25 Version 0.1.0.8
  ・一部の環境でデバイス一覧を取得できなかった問題を修正
  2010/05/27 Version 0.1.0.7
  ・一部のマウスでアプリケーション毎の設定が出来なかった問題を修正
  2010/05/10 Version 0.1.0.6
  ・SetPoint 6 への対応
  2009/11/24 Version 0.1.0.5
  ・重複している項目の選択がうまく反映されない問題を修正
  2009/09/17 Version 0.1.0.4
  ・UI 言語に英語を追加
  ・実行ファイルのサイズを削減
  ・処理の改善
  2009/09/11 Version 0.1.0.3
  ・バックアップの復元機能を追加
  ・アプリケーションのアイコンを設定
  2009/09/03 Version 0.1.0.2
  ・SetPoint を終了させられないバグを修正
  ・その他微調整
  2009/09/01 Version 0.1.0.1
  ・起動時に管理者権限に昇格するようにした
  ・左右クリックに他の動作を設定できるようにした
