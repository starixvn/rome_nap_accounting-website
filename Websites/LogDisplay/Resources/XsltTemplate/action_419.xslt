<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:param name="IsInternal" select="1"></xsl:param>
  <xsl:output method="html"/>
  <xsl:template match="acc_customerupdmasterhr_positiontaking">
    <table class="tblDesc">
      <xsl:for-each select="info">
        <tr class="headerDetail" onclick="$('#info{{tbodylogid}}').slideToggle();">
          <td colspan="6">{lbl:custInfo}</td>
        </tr>
        <tr>
          <td colspan="6">
            <div style="display:block;" id="info{{tbodylogid}}">
              <table class="tblDesc" >
                <tr>
                  <td class="colW1">{lbl:Updallmembers}</td>
                  <td class="colW2">
                    <xsl:value-of select="@updallmembers" />
                  </td>
                  <td class="colW1">{lbl:newspt}</td>
                  <td class="colW2">
                    <xsl:value-of select="@newspt" />
                  </td>
                  <td class="colW1">{lbl:IsCopyPreset_31}</td>
                  <td class="colW2">
                    <xsl:value-of select="@iscopypreset_31" />
                  </td>
                </tr>
                <tr >
                  <td class="colW1">{lbl:IsCopyPreset_41}</td>
                  <td class="colW2">
                    <xsl:value-of select="@iscopypreset_41" />
                  </td>
                  <td class="colW1">{lbl:IsCopyPreset_51}</td>
                  <td class="colW2">
                    <xsl:value-of select="@iscopypreset_51" />
                  </td>
                  <td class="colW1">{lbl:IsCopyPreset152_41}</td>
                  <td class="colW2">
                    <xsl:value-of select="@iscopypreset152_41" />
                  </td>
                </tr>
                <tr >
                  <td class="colW1">{lbl:IsCopyPreset152_51}</td>
                  <td class="colW2">
                    <xsl:value-of select="@iscopypreset152_51" />
                  </td>
                  <td class="colW1">{lbl:IsCopyPreset153_51}</td>
                  <td class="colW2">
                    <xsl:value-of select="@iscopypreset153_51" />
                  </td>
                  <td class="colW1">{lbl:SubAccName}</td>
                  <td class="colW2">
                    <xsl:value-of select="@subaccname" />
                  </td>                 
                </tr>
                <tr >
                  <td class="colW1">{lbl:UserID}</td>
                  <td class="colW2">
                    <xsl:value-of select="@userid" />
                  </td>
                  <td class="colW1">{lbl:Language}</td>
                  <td class="colW2">
                    <xsl:value-of select="@language" />
                  </td>
                  <td class="colW1">{lbl:IP}</td>
                  <td class="colW2">
                    <xsl:value-of select="@ip" />
                  </td>
                </tr>
                <tr >                  
                  <td class="colW1">{lbl:ErrCode}</td>
                  <td class="colW2">
                    <xsl:value-of select="@errcode" />
                  </td>
                  <td class="colW1">{lbl:ErrMsg}</td>
                  <td class="colW2" >
                    <xsl:value-of select="@errmsg" />
                  </td>
                  <td class="colW1"></td>
                  <td class="colW2">
                  </td>
                </tr>
              </table>
            </div>
          </td>
        </tr>
      </xsl:for-each>
      <xsl:for-each select="s">
        <tr class="headerDetailNormal" >
          <td colspan="6">{lbl:s}</td>
        </tr>
        <tr>
          <td colspan="6">
            <div >
              <table class="tblDesc" >
                <tr >
                  <td class="colW1">{lbl:v151spt_31}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v151spt_31" />
                  </td>
                  <td class="colW1">{lbl:v151spt_32}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v151spt_32" />
                  </td>
                  <td class="colW1">{lbl:v151spt_41}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v151spt_41" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">{lbl:v151spt_42}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v151spt_42" />
                  </td>
                  <td class="colW1">{lbl:v151spt_51}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v151spt_51" />
                  </td>
                  <td class="colW1">{lbl:v151spt_52}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v151spt_52" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">{lbl:v152spt_41}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v152spt_41" />
                  </td>
                  <td class="colW1">{lbl:v152spt_42}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v152spt_42" />
                  </td>
                  <td class="colW1">{lbl:v152spt_51}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v152spt_51" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">{lbl:v152spt_52}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v152spt_52" />
                  </td>
                  <td class="colW1">{lbl:v153spt_51}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v153spt_51" />
                  </td>
                  <td class="colW1">{lbl:v153spt_52}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v153spt_52" />
                  </td>
                </tr>
              </table>
            </div>
          </td>
        </tr>


      </xsl:for-each>
      <xsl:for-each select="au">
        <tr class="headerDetailNormal" >
          <td colspan="6">{lbl:au}</td>
        </tr>
        <tr>
          <td colspan="6">
            <div >
              <table class="tblDesc" >
                <tr >
                  <td class="colW1">{lbl:a_v151spt_31}</td>
                  <td class="colW2">
                    <xsl:value-of select="@a_v151spt_31" />
                  </td>
                  <td class="colW1">{lbl:a_v151spt_32}</td>
                  <td class="colW2">
                    <xsl:value-of select="@a_v151spt_32" />
                  </td>
                  <td class="colW1">{lbl:a_v151spt_41}</td>
                  <td class="colW2">
                    <xsl:value-of select="@a_v151spt_41" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">{lbl:a_v151spt_42}</td>
                  <td class="colW2">
                    <xsl:value-of select="@a_v151spt_42" />
                  </td>
                  <td class="colW1">{lbl:a_v151spt_51}</td>
                  <td class="colW2">
                    <xsl:value-of select="@a_v151spt_51" />
                  </td>
                  <td class="colW1">{lbl:a_v151spt_52}</td>
                  <td class="colW2">
                    <xsl:value-of select="@a_v151spt_52" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">{lbl:a_v152spt_41}</td>
                  <td class="colW2">
                    <xsl:value-of select="@a_v152spt_41" />
                  </td>
                  <td class="colW1">{lbl:a_v152spt_42}</td>
                  <td class="colW2">
                    <xsl:value-of select="@a_v152spt_42" />
                  </td>
                  <td class="colW1">{lbl:a_v152spt_51}</td>
                  <td class="colW2">
                    <xsl:value-of select="@a_v152spt_51" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">{lbl:a_v152spt_52}</td>
                  <td class="colW2">
                    <xsl:value-of select="@a_v152spt_52" />
                  </td>
                  <td class="colW1">{lbl:a_v153spt_51}</td>
                  <td class="colW2">
                    <xsl:value-of select="@a_v153spt_51" />
                  </td>
                  <td class="colW1">{lbl:a_v153spt_52}</td>
                  <td class="colW2">
                    <xsl:value-of select="@a_v153spt_52" />
                  </td>
                </tr>
              </table>
            </div>
          </td>
        </tr>


      </xsl:for-each>
      <xsl:for-each select="ia">
        <tr class="headerDetailNormal" >
          <td colspan="6">{lbl:isaPTonM}</td>
        </tr>
        <tr>
          <td colspan="6">
            <div >
              <table class="tblDesc" >

                <tr >
                  <td class="colW1">{lbl:isa_v151spt_31}</td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_v151spt_31" />
                  </td>
                  <td class="colW1">{lbl:isa_v151spt_32}</td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_v151spt_32" />
                  </td>

                  <td class="colW1">{lbl:isa_v151spt_41}</td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_v151spt_41" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">{lbl:isa_v151spt_42}</td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_v151spt_42" />
                  </td>
                  <td class="colW1">{lbl:isa_v151spt_51}</td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_v151spt_51" />
                  </td>
                  <td class="colW1">{lbl:isa_v151spt_52}</td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_v151spt_52" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">{lbl:isa_v152spt_41}</td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_v152spt_41" />
                  </td>
                  <td class="colW1">{lbl:isa_v152spt_42}</td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_v152spt_42" />
                  </td>
                  <td class="colW1">{lbl:isa_v152spt_51}</td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_v152spt_51" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">{lbl:isa_v152spt_52}</td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_v152spt_52" />
                  </td>
                  <td class="colW1">{lbl:isa_v153spt_51}</td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_v153spt_51" />
                  </td>
                  <td class="colW1">{lbl:isa_v153spt_52}</td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_v153spt_52" />
                  </td>
                </tr>
              </table>
            </div>
          </td>
        </tr>

      </xsl:for-each>
      <xsl:for-each select="m">
        <tr class="headerDetailNormal" >
          <td colspan="6">{lbl:m}</td>
        </tr>
        <tr>
          <td colspan="6">
            <div >
              <table class="tblDesc" >
                <tr >
                  <td class="colW1">{lbl:v151mpt_31}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v151mpt_31" />
                  </td>
                  <td class="colW1">{lbl:v151mpt_32}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v151mpt_32" />
                  </td>

                  <td class="colW1">{lbl:v151mpt_41}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v151mpt_41" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">{lbl:v151mpt_42}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v151mpt_42" />
                  </td>
                  <td class="colW1">{lbl:v151mpt_51}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v151mpt_51" />
                  </td>
                  <td class="colW1">{lbl:v151mpt_52}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v151mpt_52" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">{lbl:v152mpt_41}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v152mpt_41" />
                  </td>
                  <td class="colW1">{lbl:v152mpt_42}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v152mpt_42" />
                  </td>
                  <td class="colW1">{lbl:v152mpt_51}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v152mpt_51" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">{lbl:v152mpt_52}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v152mpt_52" />
                  </td>
                  <td class="colW1">{lbl:v153mpt_51}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v153mpt_51" />
                  </td>
                  <td class="colW1">{lbl:v153mpt_52}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v153mpt_52" />
                  </td>
                </tr>
              </table>
            </div>
          </td>
        </tr>
      </xsl:for-each>
    </table>
  </xsl:template>
</xsl:stylesheet>
